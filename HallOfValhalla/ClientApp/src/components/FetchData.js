import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';

const FetchData = () => {
    const [data, setData] = useState([]);
    const { user, getAccessTokenSilently } = useAuth0();

    const handleRegistration = (conventionId, event) => {
        event.preventDefault();

        const registerUserToConvention = async () => {
            const audience = process.env.REACT_APP_AUTH0_AUDIENCE;

            try {
                const accessToken = await getAccessTokenSilently({
                    audience: audience,
                    scope: "register:conventions",
                });

                const conventionRegistrationUrl = `${audience}conventions/${conventionId}/registrations`;

                await fetch(conventionRegistrationUrl, {
                    method: 'POST',
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ userId: user.sub })
                });

            } catch (e) {
                console.log(e.message);
            }
        };

        registerUserToConvention();
    };

    useEffect(() => {
        getAccessTokenSilently({
            audience: process.env.REACT_APP_AUTH0_AUDIENCE,
            scope: 'read:conventions'
        }).then(accessToken => {
            return fetch('api/v1/conventions', {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            })
        }).then(response => response.json())
            .then(data => setData(data));
    }, [getAccessTokenSilently]);

    return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {data.map(convention =>
                    <tr key={convention.id}>
                        <td>{convention.name}</td>
                        <td><button onClick={(event) => handleRegistration(convention.id, event)}>Register</button></td>
                    </tr>
                )}
            </tbody>
        </table>
    );
};

export default FetchData;
