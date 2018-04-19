import { Price } from './price';
export interface Product {
    id: number; 
    name: string;
    code: string; 
    edited: boolean;   
    prices: Price;
  }