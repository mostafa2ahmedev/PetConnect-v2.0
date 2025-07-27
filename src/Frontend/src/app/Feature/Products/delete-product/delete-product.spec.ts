import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteProduct } from './delete-product';

describe('DeleteProduct', () => {
  let component: DeleteProduct;
  let fixture: ComponentFixture<DeleteProduct>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteProduct]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteProduct);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
