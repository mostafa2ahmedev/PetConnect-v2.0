export interface ChatMessage {
  messageId: number;
  sentDate: string;
  message: string;
  isDeleted: boolean;
  isRead: boolean;
  readDate: string;
  messageType: number;
  attachmentUrl: string | null;
  senderId: string;
  recieverId: string;
}
