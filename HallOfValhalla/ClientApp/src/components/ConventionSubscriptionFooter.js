import React, { useEffect, useState } from 'react';

import './ConventionSubscriptionFooter.css'

const handleRegistration = (conventionId, user, getAccessTokenSilently, setIsRegistered, event) => {
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

            setIsRegistered(true);
        } catch (e) {
            console.log(e.message);
        }
    };

    registerUserToConvention();
};

const ConventionSubscriptionFooter = ({ convention, user, getAccessTokenSilently }) => {
    const [isRegistered, setIsRegistered] = useState();

    useEffect(() => {
        const fetchIsUserRegistered = async () => {
            const audience = process.env.REACT_APP_AUTH0_AUDIENCE;
            const accessToken = await getAccessTokenSilently({
                audience: audience,
                scope: "read:conventions",
            });

            const response = await fetch(`api/v1/conventions/${convention.id}/registration`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                }
            });

            const data = await response.json();

            setIsRegistered(data);
        }

        fetchIsUserRegistered()
    });

    if (isRegistered === undefined)
        return null;

    return (
        <footer className="convention-subscription-footer bg-light">
            <div className="container convention-subscription-footer__container">
                <div className="row">
                    <div className="col-9">
                        {convention.name}
                    </div>
                    <div className="col-3">
                        {
                            isRegistered ?
                                (<h5 className="text-success float-end mt-3">You are going</h5>)
                                :
                                (<button
                                    type="button"
                                    className="btn btn-danger btn-lg float-end"
                                    onClick={(event) => handleRegistration(convention.id, user, getAccessTokenSilently, setIsRegistered, event)}
                                >
                                    Register
                                </button>)
                        }
                        
                    </div>
                </div>
            </div>
        </footer>
    );
};

export default ConventionSubscriptionFooter;