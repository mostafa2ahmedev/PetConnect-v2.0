import { Component, OnInit } from '@angular/core';
import { Blog } from '../blog-models';
import { BlogService } from '../blog-service';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-all-blogs',
  imports: [DatePipe, CommonModule],
  templateUrl: './all-blogs.html',
  styleUrl: './all-blogs.css',
})
export class AllBlogs implements OnInit {
  blogs: Blog[] = [];
  loading = true;
  errorMessage = '';

  constructor(private blogService: BlogService) {}

  ngOnInit(): void {
    this.loadBlogs();
  }

  loadBlogs(): void {
    this.blogService.getAllReadWriteBlogs().subscribe({
      next: (data) => {
        this.blogs = data;
        this.loading = false;
        console.log('ddddd', data);
      },
      error: (err) => {
        this.errorMessage = 'Failed to load blogs.';
        console.error(err);
        this.loading = false;
      },
    });
  }

  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
