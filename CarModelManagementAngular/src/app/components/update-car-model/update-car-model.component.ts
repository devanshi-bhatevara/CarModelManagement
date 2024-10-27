import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { GetCarModel } from 'src/app/models/get-car-model.model';
import { CarModelService } from 'src/app/services/car-model.service';

@Component({
  selector: 'app-update-car-model',
  templateUrl: './update-car-model.component.html',
  styleUrls: ['./update-car-model.component.css']
})
export class UpdateCarModelComponent implements OnInit {
  carModel: GetCarModel = {
    carModelId: '',
    brand: '',
    class: '',
    modelName: '',
    modelCode: '',
    description: '',
    features: '',
    price: 0,
    dateOfManufacturing: ''
  };
  carModelId!: number;

  constructor(private carModelService: CarModelService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.carModelId = +this.route.snapshot.paramMap.get('id')!;
    this.loadCarModel();
  }

  loadCarModel(): void {
    this.carModelService.getCarModelById(this.carModelId).subscribe({
      next: (response: ApiResponse<GetCarModel>) => {
        if (response.success) {
          this.carModel = response.data;
          this.carModel.dateOfManufacturing = this.formatDate(this.carModel.dateOfManufacturing);

        }
      },
      error: (error) => {
        console.error('Failed to load car model:', error);
      }
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Add leading zero if needed
    const day = date.getDate().toString().padStart(2, '0'); // Add leading zero if needed
    return `${year}-${month}-${day}`;
  }

  onSubmit(): void {
    this.carModelService.updateCarModel(this.carModel).subscribe({
      next: (response: ApiResponse<string>) => {
        if (response.success) {
          this.router.navigate(['/getall']);
        }
      },
      error: (error) => {

        console.error(error)
        alert(error.error.message);
      }
    });
  }
}
