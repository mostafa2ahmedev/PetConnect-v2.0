import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateProduct } from './update-product';

describe('UpdateProduct', () => {
  let component: UpdateProduct;
  let fixture: ComponentFixture<UpdateProduct>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateProduct]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateProduct);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
