import { Component } from '@angular/core';
import { BlogService } from '../blog-service';
import { AttachmentService } from '../../../core/services/attachment-service';
import { CategoryService } from '../../categories/category-service';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { BlogPost } from '../blog-models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { AlertService } from '../../../core/services/alert-service';

@Component({
  selector: 'app-update-blog',
  imports: [CommonModule, FormsModule, QuillModule],
  templateUrl: './update-blog.html',
  styleUrl: './update-blog.css',
})
export class UpdateBlog {
  blog: any = {
    blogId: '',
    content: '',
    title: '',
    blogType: 0,
    topic: null,
    petCategoryId: null,
    excerpt: '',
    media: null,
  };

  mediaFile?: File | null;
  isEditMode = false;
  isSubmitting = false;
  errorMsg = '';
  successMsg = '';
  categories: any[] = [];
  blogTopics: any[] = [];

  constructor(
    private blogService: BlogService,
    private attachmentService: AttachmentService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    forkJoin({
      categories: this.categoryService.getCategories(),
      topics: this.blogService.getBlogTopics(),
    }).subscribe({
      next: ({ categories, topics }) => {
        this.categories = categories;
        this.blogTopics = topics;
        const blogId = this.route.snapshot.paramMap.get('id');
        if (blogId) {
          this.isEditMode = true;
          this.loadBlog(blogId);
        }
      },
      error: (err) => console.error('Error loading categories or topics', err),
    });
  }

  loadBlog(blogId: string) {
    this.blogService.getSingleBlogPost(blogId).subscribe({
      next: (data: BlogPost) => {
        const matchedCategory = this.categories.find(
          (cat) => cat.name === data.categoryName
        );
        const matchedTopic = this.blogTopics.find(
          (topic) => topic.value === data.topic
        );
        this.blog = {
          blogId: data.id,
          content: data.content,
          title: data.title,
          blogType: data.blogType,
          topic: matchedTopic ? matchedTopic.key : null, // ✅ use ID
          petCategoryId: matchedCategory ? matchedCategory.id : null, // ✅ use ID
          excerpt: data.excerpt,
          media: data.media,
        };
        console.log(this.blog);
      },
      error: (err) => console.error('Failed to load blog', err),
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.mediaFile = file;
    }
  }

  async submitBlog() {
    this.errorMsg = '';
    this.successMsg = '';

    if (!this.blog.title.trim() || !this.blog.content.trim()) {
      this.errorMsg = 'Title and content are required.';
      return;
    }

    this.isSubmitting = true;

    try {
      if (this.isEditMode) {
        // Update blog
        if (this.mediaFile) {
          this.blog.media = this.mediaFile;
        }
        console.log(this.blog);
        await this.blogService.editBlog(this.blog).toPromise();
        this.successMsg = 'Blog updated successfully!';
      } else {
        // Add blog
        if (this.mediaFile) {
          this.blog.media = this.mediaFile;
        }
        await this.blogService.addBlog(this.blog).toPromise();
        this.successMsg = 'Blog updated successfully!';
        this.alertService.success('Blog updated successfully! ');
      }
      setTimeout(() => {
        this.router.navigate(['/doc-profile/blogs']);
      }, 1000);
    } catch (error) {
      this.errorMsg = this.isEditMode
        ? 'Error updating blog. Please try again.'
        : 'Error creating blog. Please try again.';
      console.error(error);
    } finally {
      this.isSubmitting = false;
    }
  }

  getFullImageUrl(relativePath: string): string {
    return relativePath ? `${this.blogService['apiUrl']}/${relativePath}` : '';
  }
}
