import { CarModelImage } from "./car-model-image.model";

export interface CarModel {
    carModelId: number;
    brand: string;
    class: string;
    modelName: string;
    modelCode: string;
    description: string;
    features: string;
    price: number;
    dateOfManufacturing: string;
    sortOrder: number;
    images: CarModelImage[];
  }