import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { QuillModule } from 'ngx-quill';
import { BlogService } from '../blog-service';
import { AddBlogRequest } from '../blog-models';
import { AttachmentService } from '../../../core/services/attachment-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-blog',
  imports: [FormsModule, QuillModule, CommonModule],
  templateUrl: './add-blog.html',
  styleUrl: './add-blog.css',
})
export class AddBlog {
  blog: AddBlogRequest = {
    content: '',
    title: '',
    blogType: 0, // you can set default or bind from UI
  };

  mediaFile?: File | null;

  isSubmitting = false;
  errorMsg = '';
  successMsg = '';

  constructor(
    private blogService: BlogService,
    private attachmentService: AttachmentService
  ) {}

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
      // Optional: Upload media file first if exists
      if (this.mediaFile) {
        // Assuming your attachment service has an upload method returning the media URL string
        const uploadedUrl = await this.attachmentService.upload(this.mediaFile);
        // Quill content and media are separate, so here media is file attached to blog post.
        this.blog.media = this.mediaFile; // keep File in AddBlogRequest for backend multipart upload
      }

      await this.blogService.addBlog(this.blog).toPromise();
      this.successMsg = 'Blog post created successfully!';
      // Reset form
      this.blog = { content: '', title: '', blogType: 0 };
      this.mediaFile = null;
    } catch (error) {
      this.errorMsg = 'Error creating blog post. Please try again.';
      console.error(error);
    } finally {
      this.isSubmitting = false;
    }
  }
}
