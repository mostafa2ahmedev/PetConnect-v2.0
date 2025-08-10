import { Component, OnInit } from '@angular/core';
import { Blog } from '../blog-models';
import { BlogService } from '../blog-service';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Category } from '../../../models/category';
import { CategoryService } from '../../categories/category-service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-all-blogs',
  imports: [DatePipe, CommonModule, RouterModule, FormsModule],
  templateUrl: './all-blogs.html',
  styleUrl: './all-blogs.css',
})
export class AllBlogs implements OnInit {
  originalBlogs: Blog[] = [];
  blogs: Blog[] = [];
  loading = true;
  errorMessage = '';
  blogTopics: any;
  categories: Category[] = [];
  filters = {
    categoryId: '',
    topicKey: '',
    searchTerm: '',
    sortOrder: '',
  };
  showFilters = false;

  constructor(
    private blogService: BlogService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadTopics();

    // Read slugs from URL
  }

  loadBlogs(): void {
    let categoryId = this.filters.categoryId.trim();
    let topicKey = this.filters.topicKey.trim();

    // Prepare params object dynamically
    let params: any = {};
    if (categoryId !== '' && !isNaN(+categoryId)) {
      params.categoryId = +categoryId;
    }
    if (topicKey !== '' && !isNaN(+topicKey)) {
      params.topicKey = +topicKey;
    }

    this.blogService.getAllBlogs(params.topicKey, params.categoryId).subscribe({
      next: (data) => {
        console.log(data);
        this.originalBlogs = data;
        this.blogs = data;
        this.loading = false;
        this.applyClientFilters();
      },
      error: (err) => console.error('Error loading blogs:', err),
    });
  }

  loadTopics(): void {
    this.blogService.getBlogTopics().subscribe({
      next: (topics) => {
        this.blogTopics = topics; // Store for dropdown binding
        console.log('topics', this.blogTopics);
        this.route.paramMap.subscribe((params) => {
          const topicSlug = params.get('topicSlug');
          console.log('topicSlug', topicSlug);

          if (topicSlug && this.blogTopics) {
            const topic = this.blogTopics.find(
              (c: any) => c.value.toLowerCase() === topicSlug.toLowerCase()
            );

            if (topic) {
              console.log('filter topic', topic.key);
              this.filters.topicKey = topic.key.toString();
            }
          }

          this.loadBlogs();
        });
      },
      error: (err) => {
        console.error('Error loading blog topics:', err);
      },
    });
  }
  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
        this.route.paramMap.subscribe((params) => {
          const categorySlug = params.get('categorySlug');

          if (categorySlug && this.categories) {
            this.categories.forEach((cat) => {
              console.log('sds', cat.name);
            });
            const category = this.categories.find(
              (c) => c.name.toLowerCase() === categorySlug.toLowerCase()
            );

            if (category) {
              this.filters.categoryId = category.id.toString();
            }
          }

          this.loadBlogs();
        });
        console.log('cats', this.categories);
      },
      error: (err) => console.error('Error loading categories', err),
    });
  }
  onBackendFilterChange(): void {
    // Called when category/topic changes → refresh blogs from API
    this.loadBlogs();
  }

  applyClientFilters(): void {
    let filtered = [...this.originalBlogs];

    // Search filter (frontend)
    if (this.filters.searchTerm) {
      filtered = filtered.filter((b) =>
        b.title.toLowerCase().includes(this.filters.searchTerm.toLowerCase())
      );
    }

    // Sort by date (frontend)
    if (this.filters.sortOrder === 'desc') {
      filtered.sort(
        (a, b) =>
          new Date(b.postDate).getTime() - new Date(a.postDate).getTime()
      );
    } else if (this.filters.sortOrder === 'asc') {
      filtered.sort(
        (a, b) =>
          new Date(a.postDate).getTime() - new Date(b.postDate).getTime()
      );
    }

    this.blogs = filtered;
  }

  resetFilters(): void {
    this.filters = {
      categoryId: '',
      topicKey: '',
      searchTerm: '',
      sortOrder: '',
    };
    this.loadBlogs();
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
