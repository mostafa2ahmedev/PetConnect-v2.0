export interface BlogModels {}
export interface Blog {
  id: string; // UUID
  content: string; // Blog text content
  media: string; // Image/Video URL or empty
  likes: number; // Number of likes
  blogType: number; // Enum or numeric type
  postDate: string; // ISO date string
  doctorId: string; // UUID of doctor
  doctorName: string; // Doctor's display name
  doctorImgUrl: string; // URL to doctor's profile image
}
export interface BlogReadWrite {
  id: string; // UUID
  content: string;
  media: string;
  likes: number;
  comments: number; // Number of comments
  blogType: number;
  postDate: string;
  doctorId: string;
  doctorName: string;
  doctorImgUrl: string;
}
export interface BlogComment {
  id: string; // UUID
  comment: string; // Comment text
  media: string; // Optional media URL
  posterName: string; // Name of commenter
  posterImage: string; // Profile image URL of commenter
  numberOfLikes: number; // Likes on comment
  numberOfReplies: number; // Replies to comment
}
