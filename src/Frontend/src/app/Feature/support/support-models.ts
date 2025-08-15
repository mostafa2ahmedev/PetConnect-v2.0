export interface SupportModels {}
// ---- Interfaces ----

export interface CreateSupportRequestDto {
  type: number;
  message: string;
}

export interface SupportRequest {
  id: number;
  type: number;
  status: number;
  message: string;
  userId: string;
  userName: string;
  userEmail: string;
}

export interface CreateSupportResponseDto {
  message: string;
  supportRequestId: number;
  status: number;
}
