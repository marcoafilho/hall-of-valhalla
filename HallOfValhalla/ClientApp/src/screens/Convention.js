import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { useParams } from 'react-router-dom';
import ConventionSubscriptionFooter from '../components/ConventionSubscriptionFooter';
import TalkRow from '../components/TalkRow';

const Convention = () => {
    const { user, getAccessTokenSilently } = useAuth0();
    const [convention, setConvention] = useState(null);
    const { id } = useParams();

    useEffect(() => {
        const getConvention = async id => {
            const response = await fetch(`api/v1/conventions/${id}`);
            const data = await response.json();

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
                                <TalkRow
                                    key={talk.id}
                                    talk={talk}
                                    user={user}
                                    getAccessTokenSilently={getAccessTokenSilently}
                                />
                            )}
                        </tbody>
                    </table>
                </div>
                <div className="col-4">
                </div>
            </div>
            <ConventionSubscriptionFooter
                convention={convention}
                user={user}
                getAccessTokenSilently={getAccessTokenSilently}
            />
        </>
    );
};

export default Convention;
