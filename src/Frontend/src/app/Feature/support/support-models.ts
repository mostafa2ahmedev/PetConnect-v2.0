export interface SupportModels {}
// ---- Interfaces ----

export interface CreateSupportRequestDto {
  type: number;
  message: string;
}

export interface SupportRequest {
  id: number;
  supportRequestType: number;
  supportRequestStatus: string;
  message: string;
  userId: string;
  userName: string;
  userEmail: string;
}

export interface CreateSupportResponseDto {
  message: string;
  subject: string;
  supportRequestId: number;
  status: number;
}
