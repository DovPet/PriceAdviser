import { Price } from './price';
import { Eshop } from './eshop';
export interface Product {
    id: number; 
    name: string;
    code: string; 
    edited: boolean;   
    prices: Price;
    eshops: Eshop;
  }