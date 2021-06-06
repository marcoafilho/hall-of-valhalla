import React, { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import UpdateProfileForm from "./UpdateProfileForm";

const Profile = () => {
    const { user, isAuthenticated, getAccessTokenSilently, isLoading } = useAuth0();
    const [userMetadata, setUserMetadata] = useState(null);
    useEffect(() => {
        const getUserMetadata = async () => {
            const domain = process.env.REACT_APP_AUTH0_DOMAIN;

            try {
                const accessToken = await getAccessTokenSilently({
                    audience: `https://${domain}/api/v2/`,
                    scope: "read:current_user",
                });

                const userDetailsByIdUrl = `https://${domain}/api/v2/users/${user.sub}`;

                const metadataResponse = await fetch(userDetailsByIdUrl, {
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                    },
                });

                const { user_metadata } = await metadataResponse.json();

                setUserMetadata(user_metadata);
            } catch (e) {
                console.log(e.message);
            }
        };

        getUserMetadata();
    }, [user, getAccessTokenSilently]);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        isAuthenticated && (
            <div>
                <img src={user.picture} alt={user.name} />
                <h2>{user.name}</h2>
                <p>{user.email}</p>
                <h3>User Metadata</h3>
                {userMetadata ? (
                    <pre>{JSON.stringify(userMetadata, null, 2)}</pre>
                ) : (
                        "No user metadata defined"
                    )}
                <UpdateProfileForm />
                <div>
                    <a href="https://www.freepik.com/vectors/ribbon">Ribbon vector created by macrovector - www.freepik.com</a>
                </div>
            </div>
        )
    );
};

export default Profile;