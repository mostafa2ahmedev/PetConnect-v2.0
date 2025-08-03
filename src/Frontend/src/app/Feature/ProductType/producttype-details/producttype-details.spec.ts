import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducttypeDetails } from './producttype-details';

describe('ProducttypeDetails', () => {
  let component: ProducttypeDetails;
  let fixture: ComponentFixture<ProducttypeDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProducttypeDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProducttypeDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
