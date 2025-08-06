import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorEditProfile } from './doctor-edit-profile';

describe('DoctorEditProfile', () => {
  let component: DoctorEditProfile;
  let fixture: ComponentFixture<DoctorEditProfile>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoctorEditProfile]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorEditProfile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
