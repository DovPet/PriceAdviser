import { Price, SavePrice } from './price';
import { Eshop } from './eshop';

export interface Product {
    id: number; 
    name: string;
    code: string; 
    edited: boolean;   
    prices: Price;
    eshops: Eshop;
  }

  export interface SaveProduct {
    id: number; 
    code: string;
    name: string;   
  }