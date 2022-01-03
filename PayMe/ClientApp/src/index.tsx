import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import Main from './components/main/Main';
import { AppWrapper } from './context/context';

// const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={"/"}>
      <AppWrapper>
        <Main />
      </AppWrapper>
  </BrowserRouter>,
  rootElement);
