import React, { useState } from 'react';

const handleReservation = (talkId, user, getAccessTokenSilently, setIsReserved, event) => {
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

            setIsReserved(true);

        } catch (e) {
            console.log(e.message);
        }
    };

    registerUserToConvention();
};

const TalkRow = ({ talk, user, getAccessTokenSilently }) => {
    const [isReserved, setIsReserved] = useState(false);

    return (
        <tr>
            <td>
                <b>{talk.title}</b>
                <br />
                {talk.description}
            </td>
            <td>{talk.speaker}</td>
            <td>
                {
                    isReserved ?
                        (<span>Seat reserved</span>)
                        :
                        (<button
                            type="button"
                            className="btn btn-outline-success"
                            onClick={(event) => handleReservation(talk.id, user, getAccessTokenSilently, setIsReserved, event)}
                        >
                            Reserve a spot!
                        </button>)
                }
            </td>
        </tr>
    );
}

export default TalkRow;