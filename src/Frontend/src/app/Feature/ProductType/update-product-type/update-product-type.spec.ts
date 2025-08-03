import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateProductType } from './update-product-type';

describe('UpdateProductType', () => {
  let component: UpdateProductType;
  let fixture: ComponentFixture<UpdateProductType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateProductType]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateProductType);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
