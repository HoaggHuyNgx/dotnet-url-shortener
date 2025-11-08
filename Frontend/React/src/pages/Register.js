import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Auth.css';

const Register = () => {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setSuccess('');

        try {
            const response = await axios.post('http://localhost:5234/api/auth/register', {
                username,
                email,
                password
            });
            setSuccess(response.data.message || 'Registration successful!');
            // Optionally navigate to login after successful registration
            // navigate('/login');
        } catch (err) {
            if (err.response && err.response.data) {
                // Handle Identity errors which might be an array of objects
                if (Array.isArray(err.response.data)) {
                    setError(err.response.data.map(e => e.description).join('\n'));
                } else if (typeof err.response.data === 'string') {
                    setError(err.response.data);
                } else if (err.response.data.title) { // For validation errors
                    setError(err.response.data.title + ": " + Object.values(err.response.data.errors).flat().join('\n'));
                } else {
                    setError('Registration failed. Please check your details.');
                }
            } else {
                setError('Registration failed. Please try again.');
            }
        }
    };

    return (
        <div className="auth-container">
            <form className="auth-form" onSubmit={handleSubmit}>
                <h2>Register</h2>
                {error && <p className="error-message">{error}</p>}
                {success && <p className="success-message">{success}</p>}
                <div className="form-group">
                    <label>Username</label>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div className="form-group">
                    <label>Email</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div className="form-group">
                    <label>Password</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default Register;
