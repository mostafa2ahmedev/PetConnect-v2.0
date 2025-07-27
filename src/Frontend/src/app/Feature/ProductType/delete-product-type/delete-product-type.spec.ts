import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteProductType } from './delete-product-type';

describe('DeleteProductType', () => {
  let component: DeleteProductType;
  let fixture: ComponentFixture<DeleteProductType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteProductType]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteProductType);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
