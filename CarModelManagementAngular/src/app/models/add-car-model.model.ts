import { CarModelImage } from "./car-model-image.model";

export interface AddCarModel {
    brand: string;                   // Brand of the car
    class: string;                   // Class of the car
    modelName: string;               // Model name of the car
    modelCode: string;               // Unique code for the car model
    description: string;             // Description of the car
    features: string;                // Features of the car
    price: number;                   // Price of the car
    dateOfManufacturing: string;     // Date of manufacturing (format: 'YYYY-MM-DD')
    sortOrder: number;               // Sort order for displaying the car model
    images: CarModelImage[];         // Array of images for the car model
  }