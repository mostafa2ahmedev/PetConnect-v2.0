import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { QuillModule } from 'ngx-quill';
import { BlogService } from '../blog-service';
import { AddBlogRequest } from '../blog-models';
import { AttachmentService } from '../../../core/services/attachment-service';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { CategoryService } from '../../categories/category-service';

@Component({
  selector: 'app-add-blog',
  imports: [FormsModule, QuillModule, CommonModule],
  templateUrl: './add-blog.html',
  styleUrl: './add-blog.css',
})
export class AddBlog implements OnInit {
  blog: AddBlogRequest = {
    content: '',
    title: '',
    blogType: 0,
    topic: undefined,
    petCategoryId: undefined,
  };

  mediaFile?: File | null;

  isSubmitting = false;
  errorMsg = '';
  successMsg = '';

  categories: any[] = [];
  blogTopics: any[] = [];

  constructor(
    private blogService: BlogService,
    private attachmentService: AttachmentService,
    private categroyService: CategoryService
  ) {}

  ngOnInit(): void {
    forkJoin({
      categories: this.categroyService.getCategories(), // or categoryService.getCategories()
      topics: this.blogService.getBlogTopics(),
    }).subscribe({
      next: ({ categories, topics }) => {
        this.categories = categories;
        this.blogTopics = topics;
      },
      error: (err) => console.error('Error loading categories or topics', err),
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
      this.errorMsg = 'Title, content, category, and topic are required.';
      return;
    }

    this.isSubmitting = true;

    try {
      if (this.mediaFile) {
        const uploadedUrl = await this.attachmentService.upload(this.mediaFile);
        this.blog.media = this.mediaFile; // keep file for backend multipart upload
      }
      console.log(this.blog);

      await this.blogService.addBlog(this.blog).toPromise();
      this.successMsg = 'Blog post created successfully!';

      // Reset form
      this.blog = {
        content: '',
        title: '',
        blogType: 0,
        topic: undefined,
        petCategoryId: undefined,
      };
      this.mediaFile = null;
    } catch (error) {
      this.errorMsg = 'Error creating blog post. Please try again.';
      console.error(error);
    } finally {
      this.isSubmitting = false;
    }
  }
}
