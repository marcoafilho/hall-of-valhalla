import React, { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";

export const useInput = initialValue => {
    const [value, setValue] = useState(initialValue);

    return {
        value,
        setValue,
        reset: () => setValue(""),
        bind: {
            value,
            onChange: event => {
                setValue(event.target.value);
            }
        }
    };
};

const UpdateProfileForm = () => {
    const { user, getAccessTokenSilently } = useAuth0();
    const { value:phoneNumber, bind:bindPhoneNumber, reset:resetPhoneNumber } = useInput('');
    const { value:address, bind:bindAdress, reset:resetAddress } = useInput('');

    const handleSubmit = (evt) => {
        evt.preventDefault();

        const setUserMetadata = async () => {
            const domain = process.env.REACT_APP_AUTH0_DOMAIN;

            try {
                const accessToken = await getAccessTokenSilently({
                    audience: `https://${domain}/api/v2/`,
                    scope: "update:current_user_metadata",
                });

                const updateUserDetailsUrl = `https://${domain}/api/v2/users/${user.sub}`;

                await fetch(updateUserDetailsUrl, {
                    method: 'PATCH',
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ user_metadata: { phone_number: phoneNumber, address: address } })
                });

            } catch (e) {
                console.log(e.message);
            }
        };

        setUserMetadata();
    }

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Phone Number: <input type="text" {...bindPhoneNumber} />
            </label>
            <label>
                Address: <textarea {...bindAdress}></textarea>
            </label>
            <br />
            <input type="submit" value="Submit" />
        </form>
    )
};

export default UpdateProfileForm;