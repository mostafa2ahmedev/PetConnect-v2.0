import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Breeds } from './breeds';

describe('Breeds', () => {
  let component: Breeds;
  let fixture: ComponentFixture<Breeds>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Breeds]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Breeds);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
