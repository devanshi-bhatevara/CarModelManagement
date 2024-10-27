import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddCarModel } from 'src/app/models/add-car-model.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { CarModelService } from 'src/app/services/car-model.service';

@Component({
  selector: 'app-add-car-model',
  templateUrl: './add-car-model.component.html',
  styleUrls: ['./add-car-model.component.css']
})
export class AddCarModelComponent implements OnInit {

  carModel: AddCarModel = {
    brand: '',
    class: '',
    modelName: '',
    modelCode: '',
    description: '',
    features: '',
    price: 0,
    dateOfManufacturing: '',
    sortOrder: 0,
    images: [] as { imageData: string, imageName: string, imageType: string }[]
  }
  constructor(private carModelService: CarModelService, private router: Router) { }

  ngOnInit(): void {
  }

  onFileChange(event: any) {
    const files: FileList = event.target.files;
    this.carModel.images = []; 
    const maxFileSize = 5 * 1024 * 1024; // 5 MB in bytes

    if (files.length > 0) {
        const validFiles: File[] = []; // Store valid files to display

        Array.from(files).forEach((file: File) => {
            // Check if the file size is greater than 5 MB
            if (file.size > maxFileSize) {
                alert(`File "${file.name}" is larger than 5 MB and will not be uploaded.`);
            } else {
                validFiles.push(file); // Add valid files to the array

                const reader = new FileReader();
                reader.onload = () => {
                    const base64String = reader.result as string;
                    this.carModel.images.push({
                        imageData: base64String.split(',')[1], 
                        imageName: file.name,
                        imageType: file.type
                    });
                };
                reader.onerror = (error) => {
                    console.error('Error reading file:', error);
                };
                reader.readAsDataURL(file); // Read the file as a Base64 string
            }
        });

        // If there are invalid files, reset the file input
        if (validFiles.length < files.length) {
            event.target.value = ''; // Clear the file input
        }
    }
}


  onSubmit() {
    this.carModelService.addCarModel(this.carModel)
    .subscribe({
      next: (response :ApiResponse<string>)=> {
        if(response.success){
        this.router.navigate(['/getall']);     
        }
      },
      error: (error)=> {
        alert(error.error.message)        
      }
  });
  }

}
