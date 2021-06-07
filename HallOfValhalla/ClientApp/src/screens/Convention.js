import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useParams } from 'react-router-dom';
import ConventionSubscriptionFooter from '../components/ConventionSubscriptionFooter';

const handleRegistration = (conventionId, user, getAccessTokenSilently, event) => {
    event.preventDefault();

    const registerUserToConvention = async () => {
        const audience = process.env.REACT_APP_AUTH0_AUDIENCE;

        try {
            const accessToken = await getAccessTokenSilently({
                audience: audience,
                scope: "register:conventions",
            });

            const conventionRegistrationUrl = `${audience}conventions/${conventionId}/registration`;

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

const handleReservation = (talkId, user, getAccessTokenSilently, event) => {
    event.preventDefault();

    const registerUserToConvention = async () => {
        const audience = process.env.REACT_APP_AUTH0_AUDIENCE;

        try {
            const accessToken = await getAccessTokenSilently({
                audience: audience,
                scope: "reserve:talks",
            });

            const conventionRegistrationUrl = `${audience}talks/${talkId}/reservations`;

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

const Convention = () => {
    const { user, getAccessTokenSilently } = useAuth0();
    const [convention, setConvention] = useState(null);
    const { id } = useParams();

    useEffect(() => {
        const getConvention = async id => {
            const response = await fetch(`api/v1/conventions/${id}`);
            const data = await response.json();

            const audience = process.env.REACT_APP_AUTH0_AUDIENCE;
            const accessToken = await getAccessTokenSilently({
                audience: audience,
                scope: "read:conventions",
            });

            const registration = await fetch(`api/v1/conventions/${id}/registration`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                }
            });

            setConvention(data);
        }
        
        getConvention(id);
    }, [id]);

    if (convention === null)
        return <div>Loading...</div>;

    return (
        <>
            <div className="row">
                <div className="col-8">
                    <h1>{convention.name}</h1>
                    <h2>Talks</h2>
                    <table className="table">
                        <tbody>
                            <tr>
                                <th>Title</th>
                                <th>Speaker</th>
                                <th>Actions</th>
                            </tr>
                            {convention.talks.map(talk =>
                                <tr key={talk.id}>
                                    <td>
                                        <b>{talk.title}</b>
                                        <br />
                                        {talk.description}
                                    </td>
                                    <td>{talk.speaker}</td>
                                    <td>
                                        <button
                                            type="button"
                                            className="btn btn-outline-success"
                                            onClick={(event) => handleReservation(talk.id, user, getAccessTokenSilently, event)}>
                                            Reserve a spot!
                                        </button>
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
                <div className="col-4">
                </div>
            </div>
            <ConventionSubscriptionFooter
                name={convention.name}
                onRegister={(event) => handleRegistration(convention.id, user, getAccessTokenSilently, event)}
            />
        </>
    );
};

export default Convention;
