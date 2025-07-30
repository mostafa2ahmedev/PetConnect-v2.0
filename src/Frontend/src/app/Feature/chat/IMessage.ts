export interface IMessage {
  attachmentUrl: string | null;
  isDeleted: boolean;
  isRead: boolean;
  message: string | null;
  messageType: 'Text' | 'File';
  senderId: string | null;
  recieverId: string | null;
}