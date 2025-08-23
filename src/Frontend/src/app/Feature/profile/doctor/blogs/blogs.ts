import { Component } from '@angular/core';
import { Blog } from '../../../Blog/blog-models';
import { BlogService } from '../../../Blog/blog-service';
import { AccountService } from '../../../../core/services/account-service';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AlertService } from '../../../../core/services/alert-service';

@Component({
  selector: 'app-blogs',
  imports: [RouterModule, CommonModule],
  templateUrl: './blogs.html',
  styleUrl: './blogs.css',
})
export class Blogs {
  blogs: Blog[] = [];
  loadingBlogs = true;
  errorMessage = '';
  constructor(
    private blogServie: BlogService,
    private accountService: AccountService,
    private alertService: AlertService
  ) {}
  ngOnInit(): void {
    const user = this.accountService.jwtTokenDecoder();
    this.loadUserBlogs(user.userId);
  }
  loadUserBlogs(UserId: string): void {
    this.blogServie.getBlogsByUserId(UserId).subscribe({
      next: (data) => {
        this.blogs = data;
        this.loadingBlogs = false;
        console.log('blogs', this.blogs);
      },
      error: (err) => {
        this.errorMessage = 'Failed to load blogs';
        this.loadingBlogs = false;
      },
    });
  }
  confirmDelete(blogId: string): void {
    this.alertService
      .confirm(
        'Are you sure you want to delete this blog post?',
        'Confirm Deletion'
      )
      .then((isConfirmed) => {
        if (isConfirmed) {
          this.blogServie.deleteBlog(blogId).subscribe({
            next: () => {
              this.blogs = this.blogs.filter((b) => b.id !== blogId);
            },
            error: () => {
              this.alertService.error('Failed to delete blog');
            },
          });
        }
      });
  }
  getFullImageUrl(relativePath: string): string {
    if (typeof relativePath == 'string')
      return `https://localhost:7102/${relativePath}`;
    return '';
  }
}
