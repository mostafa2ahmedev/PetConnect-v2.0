export interface BlogModels {}
export interface Blog {
  id: string;
  title: string;
  excerpt: string;
  media: string;
  likes: number;
  comments: number;
  blogType: number;
  postDate: string;
  doctorId: string;
  doctorName: string;
  doctorImgUrl: string;
}
export interface BlogPost {
  id: string;
  title: string;
  excerpt: string;
  content: string;
  media: string;
  likes: number;
  comments: number;
  blogType: number;
  postDate: string;
  doctorId: string;
  doctorName: string;
  doctorImgUrl: string;
  isLikedByUser: boolean;
}
export interface BlogComment {
  id: string; // UUID
  comment: string; // Comment text
  media: string; // Optional media URL
  posterName: string; // Name of commenter
  posterImage: string; // Profile image URL of commenter
  numberOfLikes: number; // Likes on comment
  numberOfReplies: number; // Replies to comment
  isLikedByUser: boolean;
  showReplies: boolean;
  replies: BlogCommentReply[];
}

export interface BlogCommentReply {
  id: string; // UUID
  reply: string;
  media: 'string';
  posterName: 'string';
  posterImage: 'string';
  numberOfLikes: number;
  isLikedByUser: boolean;
}
export interface AddBlogRequest {
  content: string; // required
  media?: File | null; // optional, binary file
  title: string; // required
  excerpt?: string; // optional
  blogType: number; // required, integer
}

// add-comment.model.ts
export interface AddCommentRequest {
  blogId: string; // required, UUID
  comment: string; // optional text
  media?: File | null; // optional, binary file
}

// add-reply.model.ts
export interface AddReplyRequest {
  commentId: string; // required, UUID
  reply: string; // optional text
  media?: File | null; // optional, binary file
}
