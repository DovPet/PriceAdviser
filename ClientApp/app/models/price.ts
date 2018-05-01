export interface Price {
    id: number; 
    value: number;
    updatedAt: string | null;
    eshopId: number;  
  }
  
    export interface SavePrice {
    id: number; 
    value: number;   
    eshopId: number; 
    updatedAt: string | null;
    productId: number;   
    edited: boolean;
  }