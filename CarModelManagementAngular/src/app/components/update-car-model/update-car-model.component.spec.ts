import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateCarModelComponent } from './update-car-model.component';

describe('UpdateCarModelComponent', () => {
  let component: UpdateCarModelComponent;
  let fixture: ComponentFixture<UpdateCarModelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateCarModelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateCarModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
