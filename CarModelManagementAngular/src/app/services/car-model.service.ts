import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CarModel } from '../models/car-model.model';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { AddCarModel } from '../models/add-car-model.model';
import { GetCarModel } from '../models/get-car-model.model';

@Injectable({
  providedIn: 'root'
})
export class CarModelService {
   private apiUrl = 'http://localhost:5162/api/CarModel/'
  constructor(private http: HttpClient) { }

  getAllCarModels(search? : string): Observable<ApiResponse<CarModel[]>> {
    if(search == null)
    {
    return this.http.get<ApiResponse<CarModel[]>>(`${this.apiUrl}GetAllCarModels`);
    }
    else
    {
      return this.http.get<ApiResponse<CarModel[]>>(`${this.apiUrl}GetAllCarModels?search=${search}`);
    }
}

addCarModel(carModel: AddCarModel): Observable<ApiResponse<string>> {
  return this.http.post<ApiResponse<string>>(`${this.apiUrl}AddCarModelWithImages`, carModel);
}

getCarModelById(carModelId: number): Observable<ApiResponse<GetCarModel>> {
  return this.http.get<ApiResponse<GetCarModel>>(`${this.apiUrl}GetCarModelById/${carModelId}`);
}

updateCarModel(carModel: GetCarModel): Observable<ApiResponse<string>> {
  return this.http.put<ApiResponse<string>>(`${this.apiUrl}UpdateCarModel`, carModel);
}

deleteCarModel(carModelId: number): Observable<ApiResponse<string>> {
  return this.http.delete<ApiResponse<string>>(`${this.apiUrl}DeleteCarModel/${carModelId}`);
}
}
