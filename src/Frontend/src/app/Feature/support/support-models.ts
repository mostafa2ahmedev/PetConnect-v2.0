export interface SupportModels {}

// ---- Interfaces ----

// Create a Support Request
export interface CreateSupportRequestDto {
  type: number; // integer ($int32) - request type
  message: string; // required
  pictureUrl?: File; // optional file upload
}

// Create a Follow-Up Support Request
export interface CreateFollowUpSupportRequestDto {
  message: string; // required
  supportRequestId: number; // required (links to original request)
  pictureUrl?: File; // optional file upload
}

// Create an Admin Support Response
export interface CreateSupportResponseDto {
  message: string; // required
  subject: string; // required
  supportRequestId: number; // required
  status: number; // required (status enum/int)
  pictureUrl?: File; // optional file upload
}

// Support Request (for listing in admin/user views)
export interface SupportRequest {
  id: number;
  supportRequestType: string; // string in API schema
  supportRequestStatus: string; // string in API schema
  priority: string;
  createdAt: string;
  lastActivity: string;
  message: string;
  userId: string;
  userName: string;
  userEmail: string;
}
export interface UserSupportRequestInfo {
  id: number;
  type: string;
  status: string;
  priority: string;
  message: string;
  createdAt: string;
  lastActivity: string;
}
// Follow-up from user
export interface SupportFollowUp {
  id: number;
  supportRequestId: number;
  message: string;
  createdAt: string;
  pictureUrl?: string;
}

// Response from admin/support team
export interface SupportResponse {
  id: number;
  supportRequestId: number;
  subject: string;
  message: string;
  status: string; // could also be number if enum
  createdAt: string;
  pictureUrl?: string;
}
// ---- User Support Request Details (full thread) ----
export interface UserSupportRequestDetails {
  id: number;
  type: string;
  status: string;
  priority: string;
  createdAt: string;
  lastActivity: string;
  message: string;
  userId: string;
  userName: string;
  userEmail: string;
  pictureUrl: string;
  supportMessages: SupportMessage[];
  followUps?: SupportFollowUp[];
  responses?: SupportResponse[];
}
export interface UserSupportRequestDetails2 {
  id: number;
  status: string;

  createdAt: string;
  lastActivity: string;

  message: string;

  supportMessages: SupportMessage[];
}
export interface SupportMessage {
  createdAt: string;
  message: string;
  pictureUrl?: string;
  sender: string;
}
// ---- Update Status / Priority ----
export interface UpdateRequestStatusPriorityDto {
  supportRequestId: number;
  status?: string; // optional if only priority is updated
  priority?: string; // optional if only status is updated
}
export interface EnumOption {
  key: number | string;
  value: string;
}
