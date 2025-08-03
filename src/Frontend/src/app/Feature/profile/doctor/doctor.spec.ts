import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Doctor } from './doctor';

describe('Doctor', () => {
  let component: Doctor;
  let fixture: ComponentFixture<Doctor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Doctor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Doctor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
