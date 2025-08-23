import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BlogService } from '../blog-service';
import {
  AddCommentRequest,
  AddReplyRequest,
  BlogComment,
  BlogCommentReply,
  BlogPost,
} from '../blog-models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';
import { AttachmentService } from '../../../core/services/attachment-service';

@Component({
  selector: 'app-single-post',
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './single-post.html',
  styleUrl: './single-post.css',
})
export class SinglePost {
  blogPost!: BlogPost;
  isAuth: boolean = false;
  userId: string = '';
  blogId: string | null = '';
  comments: BlogComment[] = [];
  newComment: AddCommentRequest = {
    blogId: '',
    comment: '',
    media: null,
  };
  newReply: AddReplyRequest = {
    commentId: '',
    reply: '',
    media: null,
  };
  loading: boolean = true;
  imagePreview: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private blogService: BlogService,
    private accountService: AccountService,
    public attachmentService: AttachmentService
  ) {}

  ngOnInit(): void {
    // Get ID from URL
    this.blogId = this.route.snapshot.paramMap.get('id');
    this.isAuth = this.accountService.isAuthenticated();
    this.userId = this.accountService.getUserId();
    if (this.blogId) {
      this.newComment.blogId = this.blogId;
      this.getPostDetails(this.blogId);
      this.loadComments(this.blogId);
    }
  }

  getPostDetails(blogId: string) {
    this.blogService.getSingleBlogPost(blogId).subscribe({
      next: (blog: BlogPost) => {
        this.blogPost = blog;
        console.log('Blog Post Details:', blog);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching blog post:', err);
      },
    });
  }
  loadComments(blogId: string) {
    this.blogService.getBlogComments(blogId).subscribe({
      next: (comments: BlogComment[]) => {
        this.comments = comments;
        console.log('Comments for post', blogId, comments);
      },
      error: (err) => {
        console.error('Error fetching comments', err);
      },
    });
  }
  // ---------- LIKE METHODS ----------
  likePost(blogId: string) {
    this.blogService.toggleBlogLike(blogId).subscribe({
      next: () => {
        // Toggle like count instantly
        if (this.blogPost.isLikedByUser) {
          this.blogPost.likes--;
        } else {
          this.blogPost.likes++;
        }
        this.blogPost.isLikedByUser = !this.blogPost.isLikedByUser;
      },
      error: (err) => console.error('Error liking post:', err),
    });
  }

  likeComment(commentId: string) {
    this.blogService.toggleCommentLike(commentId).subscribe({
      next: (res) => {
        console.log('Comment liked/unliked:', res);
        const comment = this.comments.find((c) => c.id === commentId);
        if (comment) {
          // Toggle status based on response or flip it manually
          comment.isLikedByUser = res.isLiked ?? !comment.isLikedByUser;

          // Update like count
          if (comment.isLikedByUser) {
            comment.numberOfLikes = (comment.numberOfLikes || 0) + 1;
          } else {
            comment.numberOfLikes = Math.max(
              (comment.numberOfLikes || 0) - 1,
              0
            );
          }
        }
      },
      error: (err) => console.error('Error liking comment:', err),
    });
  }
  loadReplies(commentId: string) {
    // Find the comment in your comments array
    const comment = this.comments.find((c) => c.id === commentId);
    if (!comment) return;

    // Toggle the showReplies flag
    comment.showReplies = !comment.showReplies;

    // If we're showing replies and they haven't been loaded yet, fetch them
    if (
      comment.showReplies &&
      (!comment.replies || comment.replies.length === 0)
    ) {
      this.blogService.getReplies(commentId).subscribe({
        next: (res) => {
          console.log('Replies loaded:', commentId, res);
          comment.replies = res; // append or set
        },
        error: (err) => console.error('Error loading replies:', err),
      });
    }
  }
  likeReply(replyId: string, commentId: string) {
    // Find the reply object in your replies list first
    // (Assuming you have a way to access replies, e.g. in a nested array)
    const reply = this.findReplyById(replyId);
    if (!reply) return;

    this.blogService.toggleReplyLike(replyId).subscribe({
      next: () => {
        // Toggle the isLikedByUser flag
        reply.isLikedByUser = !reply.isLikedByUser;
        const comment = this.comments.find((c) => c.id === commentId);

        // Update the number of likes accordingly
        if (reply.isLikedByUser) {
          reply.numberOfLikes = (reply.numberOfLikes || 0) + 1;
        } else {
          reply.numberOfLikes = Math.max((reply.numberOfLikes || 1) - 1, 0);
        }
        this.blogService.getReplies(commentId).subscribe({
          next: (res) => {
            console.log('Replies loaded:', commentId, res);
            const comment = this.comments.find((c) => c.id === commentId);
            if (comment) comment.replies = res; // append or set
          },
          error: (err) => console.error('Error loading replies:', err),
        });
      },
      error: (err) => console.error('Error liking reply:', err),
    });
  }

  // Helper method to find reply by id (you need to implement this based on your data structure)
  findReplyById(replyId: string) {
    // Example if replies are nested inside comments:
    for (const comment of this.comments) {
      if (comment.replies) {
        const found = comment.replies.find((r) => r.id === replyId);
        if (found) return found;
      }
    }
    return null;
  }
  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.newComment.media = file;

      // Create preview
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }
  clearImage() {
    this.imagePreview = null;
    this.newComment.media = null;

    // Optionally reset the file input element value as well to allow re-selecting the same file
    const fileInput = document.getElementById(
      'fileUpload'
    ) as HTMLInputElement | null;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  addComment() {
    if (!this.newComment.comment.trim()) {
      console.error('Comment is required');
      return;
    }

    // Ensure blogId is attached
    if (typeof this.blogId === 'string') {
      this.loadComments(this.blogId);
    }
    this.blogService.addComment(this.newComment).subscribe({
      next: (res) => {
        console.log('New comment added:', res);
        this.clearImage();
        if (typeof this.blogId === 'string') {
          this.loadComments(this.blogId);
        }

        // Reset form
        this.newComment.comment = '';
        this.newComment.media = null;
      },
      error: (err) => console.error('Error adding comment:', err),
    });
  }

  addReply(commentId: string) {
    // Ensure reply is not empty
    if (!this.newReply.reply.trim()) {
      console.error('Reply is required');
      return;
    } else {
      this.newReply.commentId = commentId;
    }

    this.blogService.addReply(this.newReply).subscribe({
      next: (res) => {
        console.log('New reply added:', res);

        this.blogService.getReplies(commentId).subscribe({
          next: (res) => {
            console.log('Replies loaded:', commentId, res);
            const comment = this.comments.find((c) => c.id === commentId);
            if (comment) {
              comment.replies = res;
              comment.numberOfReplies += 1;
            } // append or set
          },
          error: (err) => console.error('Error loading replies:', err),
        });

        // Clear the reply input after posting
        this.newReply.reply = '';
        this.newReply.media = null;
      },
      error: (err) => console.error('Error adding reply:', err),
    });
  }
  deleteComment(commentId: string) {
    this.blogService.deleteComment(commentId).subscribe({
      next: () => {
        console.log('Comment deleted:', commentId);

        // Remove from local list
        this.comments = this.comments.filter((c) => c.id !== commentId);
      },
      error: (err) => console.error('Error deleting comment:', err),
    });
  }

  deleteReply(commentId: string, replyId: string) {
    this.blogService.deleteReply(replyId).subscribe({
      next: () => {
        console.log('Reply deleted:', replyId);

        // Find the comment and update its replies list
        const comment = this.comments.find((c) => c.id === commentId);
        if (comment) {
          comment.replies = comment.replies.filter((r) => r.id !== replyId);
          comment.numberOfReplies = Math.max(0, comment.numberOfReplies - 1);
        }
      },
      error: (err) => console.error('Error deleting reply:', err),
    });
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
