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

export interface BlogComment {
  id: string; // UUID
  comment: string; // Comment text
  media: string; // Optional media URL
  posterName: string; // Name of commenter
  posterImage: string; // Profile image URL of commenter
  numberOfLikes: number; // Likes on comment
  numberOfReplies: number; // Replies to comment
}
