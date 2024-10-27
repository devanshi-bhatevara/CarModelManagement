import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CarModel } from 'src/app/models/car-model.model';
import { CarModelService } from 'src/app/services/car-model.service';

@Component({
  selector: 'app-get-all',
  templateUrl: './get-all.component.html',
  styleUrls: ['./get-all.component.css']
})
export class GetAllComponent implements OnInit {

  carModels: CarModel[] = [];
  searchTerm: string = '';
  constructor(private carModelService: CarModelService, private router: Router) { }

  ngOnInit(): void {
    this.loadCarModels(); // Initial load without search term
  }

  loadCarModels(): void {
    this.carModelService.getAllCarModels(this.searchTerm).subscribe({
      next: (response) => {
        console.log(response);
        this.carModels = response.data;
      },
      error: (error) => {
        this.carModels = [];
        console.error('Error', error);
      }
    });
  }

  onSearch(): void {
    this.loadCarModels(); // Trigger search when input changes
  }

  clearSearch(): void {
    this.searchTerm = ''; // Clear the search term
    this.loadCarModels(); // Reload car models without the search filter
  }

  deleteCarModel(carModelId: number): void {
    const confirmation = confirm('Are you sure you want to delete this car model?');

    if (confirmation) {
        this.carModelService.deleteCarModel(carModelId).subscribe({
            next: (response) => {
                console.log(response.message);
                this.loadCarModels(); 
            },
            error: (error) => {
                console.error('Error deleting car model:', error);
            }
        });
    } else {
        console.log('Deletion canceled.');
    }
}


onUpdate(id: number): void {
  this.router.navigate(['/update', id]);
}

}
