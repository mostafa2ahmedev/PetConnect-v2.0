import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorLearnMore } from './doctor-learn-more';

describe('DoctorLearnMore', () => {
  let component: DoctorLearnMore;
  let fixture: ComponentFixture<DoctorLearnMore>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorLearnMore]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorLearnMore);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
