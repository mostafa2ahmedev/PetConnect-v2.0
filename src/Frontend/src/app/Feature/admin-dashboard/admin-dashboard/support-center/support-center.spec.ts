import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportCenter } from './support-center';

describe('SupportCenter', () => {
  let component: SupportCenter;
  let fixture: ComponentFixture<SupportCenter>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SupportCenter]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportCenter);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
