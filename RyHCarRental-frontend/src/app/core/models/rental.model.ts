export type RentalStatus = 'Active' | 'Completed' | 'Cancelled';

export interface RentalDetail {
  id: number;
  rentalId: number;
  vehicleId: number;
  vehicleModel: string | null;
  subTotal: number;
}

export interface Rental {
  id: number;
  startDate: string;
  endDate: string;
  totalCost: number;
  status: RentalStatus;
  customerId: number;
  customerName: string | null;
  rentalDetails: RentalDetail[];
}

export interface RentalRequest {
  startDate: string;
  endDate: string;
  customerId: number;
  vehicleIds: number[];
}