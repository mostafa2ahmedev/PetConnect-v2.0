export interface MessengerContact {
  receiverName: string;
  imageURL: string;
  lastMessage: string;
  lastMessageDate: string; // or Date, if you want to convert it later
  isOnline: boolean;
  isRead: boolean;
  userId: string;
}
