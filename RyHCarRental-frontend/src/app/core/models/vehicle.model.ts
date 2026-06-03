export type VehicleStatus = 'Available' | 'Rented' | 'InMaintenance';

export interface Vehicle {
  id: number;
  model: string;
  plate: string;
  year: number;
  dailyRate: number;
  status: VehicleStatus;
  vehicleTypeId: number;
  vehicleTypeName: string | null;
  branchId: number;
  branchName: string | null;
}

export interface VehicleRequest {
  model: string;
  plate: string;
  year: number;
  dailyRate: number;
  status: VehicleStatus;
  vehicleTypeId: number;
  branchId: number;
}