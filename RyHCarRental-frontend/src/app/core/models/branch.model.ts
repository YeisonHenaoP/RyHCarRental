export interface Branch {
  id: number;
  name: string;
  address: string;
  city: string;
  phone: string;
}

export interface BranchRequest {
  name: string;
  address: string;
  city: string;
  phone: string;
}