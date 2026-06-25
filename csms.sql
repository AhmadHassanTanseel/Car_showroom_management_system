CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    otp INT
);

CREATE TABLE employees (
    employee_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE
);

CREATE TABLE cars (
    registration_id VARCHAR(20) PRIMARY KEY,
    color VARCHAR(30),
    model VARCHAR(50),
    year INTEGER,
    brand VARCHAR(50),
    engine_type VARCHAR(30),
    price NUMERIC(10,2),
    availability VARCHAR(20),
    is_active BOOLEAN DEFAULT TRUE,
    car_image BYTEA
);

CREATE TABLE sales (
    sales_id SERIAL PRIMARY KEY,
    registration_id VARCHAR(20) NOT NULL,
    buyer_id INT NOT NULL,
    price NUMERIC(10,2) NOT NULL,
    sale_date DATE NOT NULL,
    payment_method VARCHAR(50),

    FOREIGN KEY (registration_id)
        REFERENCES cars(registration_id),

    FOREIGN KEY (buyer_id)
        REFERENCES users(user_id)
);

CREATE TABLE bookings (
    booking_id SERIAL PRIMARY KEY,
    registration_id VARCHAR(20) NOT NULL,
    buyer_id INT NOT NULL,
    booking_date DATE NOT NULL,
    payment_method VARCHAR(50),
    status VARCHAR(20) DEFAULT 'booked',

    FOREIGN KEY (registration_id)
        REFERENCES cars(registration_id),

    FOREIGN KEY (buyer_id)
        REFERENCES users(user_id)
);

-- View Tables

SELECT * FROM users;
SELECT * FROM employees;
SELECT * FROM cars;
SELECT * FROM sales;
SELECT * FROM bookings;

-- Example Update

UPDATE cars
SET availability = 'yes'
WHERE availability = 'available';