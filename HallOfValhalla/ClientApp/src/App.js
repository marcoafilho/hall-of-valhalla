import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import FetchData from './components/FetchData';
import Profile from './components/Profile';
import Conventions from './screens/Conventions';
import Convention from './screens/Convention';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route path='/' exact component={Conventions} />
                <Route path='/conventions/:id' component={Convention} />
                <Route path='/profile' component={Profile} />
                <Route path='/fetch-data' component={FetchData} />
            </Layout>
        );
    }
}
