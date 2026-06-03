export interface Customer {
  id: number;
  fullName: string;
  documentId: string;
  email: string;
  phone: string;
}

export interface CustomerRequest {
  fullName: string;
  documentId: string;
  email: string;
  phone: string;
}