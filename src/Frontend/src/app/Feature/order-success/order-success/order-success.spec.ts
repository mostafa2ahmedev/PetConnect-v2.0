import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderSuccess } from './order-success';

describe('OrderSuccess', () => {
  let component: OrderSuccess;
  let fixture: ComponentFixture<OrderSuccess>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderSuccess]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderSuccess);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
