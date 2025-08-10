import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { map, Observable } from 'rxjs';
import {
  AddBlogRequest,
  AddCommentRequest,
  AddReplyRequest,
  Blog,
  BlogComment,
  BlogCommentReply,
  BlogPost,
} from './blog-models';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ApiResponse } from '../../models/api-response';

@Injectable({
  providedIn: 'root',
})
export class BlogService {
  private apiUrl = environment.apiBaseUrl; // e.g. "https://localhost:7102"

  constructor(private http: HttpClient) {}
  getAllBlogs(
    topic?: number | null,
    categoryId?: number | null
  ): Observable<Blog[]> {
    let params = new HttpParams();

    if (topic !== undefined && topic !== null) {
      params = params.set('Topic', topic.toString());
    }
    if (categoryId !== undefined && categoryId !== null) {
      params = params.set('CategoryId', categoryId.toString());
    }

    return this.http
      .get<ApiResponse<Blog[]>>(`${this.apiUrl}/Blog/AllBlogs`, { params })
      .pipe(map((res) => res.data));
  }

  getSingleBlogPost(id: string): Observable<BlogPost> {
    return this.http
      .get<ApiResponse<BlogPost>>(`${this.apiUrl}/Blog/Blog/${id}`)
      .pipe(map((res) => res.data));
  }

  /** Get all comments for a specific blog post */
  getBlogComments(blogId: string): Observable<BlogComment[]> {
    return this.http
      .get<ApiResponse<BlogComment[]>>(
        `${this.apiUrl}/Blog/ReadWriteBlogs/Comments/${blogId}`
      )
      .pipe(map((res) => res.data));
  }

  getReplies(commentId: string): Observable<BlogCommentReply[]> {
    return this.http
      .get<ApiResponse<BlogCommentReply[]>>(
        `${this.apiUrl}/blog/Replies/${commentId}`
      )
      .pipe(map((res) => res.data));
  }

  toggleBlogLike(blogId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/blog/BlogLike/${blogId}`, {});
  }

  // Toggle like for a comment
  toggleCommentLike(commentId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/blog/CommentLike/${commentId}`, {});
  }

  // Toggle like for a reply
  toggleReplyLike(replyId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/blog/ReplyLike/${replyId}`, {});
  }

  // Add new blog
  addBlog(blog: AddBlogRequest): Observable<any> {
    const formData = new FormData();
    formData.append('Content', blog.content);
    formData.append('Title', blog.title);
    formData.append('BlogType', blog.blogType.toString());

    if (blog.excerpt) {
      formData.append('Excerpt', blog.excerpt);
    }

    if (blog.media) {
      formData.append('Media', blog.media);
    }

    if (blog.petCategoryId !== undefined && blog.petCategoryId !== null) {
      formData.append('PetCategoryId', blog.petCategoryId.toString());
    }

    if (blog.topic !== undefined && blog.topic !== null) {
      formData.append('Topic', blog.topic.toString());
    }

    return this.http.post(`${this.apiUrl}/blog/NewBlog`, formData);
  }
  deleteBlog(blogId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/blog/DeleteBlog/${blogId}`);
  }
  // Add new comment
  addComment(request: AddCommentRequest) {
    const formData = new FormData();
    formData.append('blogId', request.blogId);
    formData.append('comment', request.comment);
    if (request.media) {
      formData.append('media', request.media);
    }
    return this.http.post(`${this.apiUrl}/blog/NewComment`, formData);
  }

  // Add new reply
  addReply(replyData: AddReplyRequest): Observable<any> {
    const formData = new FormData();
    formData.append('CommentId', replyData.commentId);
    if (replyData.reply) formData.append('Reply', replyData.reply);
    if (replyData.media) formData.append('Media', replyData.media);

    return this.http.post(`${this.apiUrl}/Blog/NewReply`, formData);
  }
  deleteComment(commentId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/blog/DeleteComment/${commentId}`);
  }

  // Delete reply
  deleteReply(replyId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/blog/DeleteReply/${replyId}`);
  }

  getBlogTopics(): Observable<{ key: number; value: string }[]> {
    return this.http.get<{ key: number; value: string }[]>(
      `${this.apiUrl}/Enums/blog-topics`
    );
  }
}
