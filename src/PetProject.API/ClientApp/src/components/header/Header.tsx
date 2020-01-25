﻿import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import ButtonBase from '@material-ui/core/ButtonBase';
import Typography from '@material-ui/core/Typography';
import petSearch from '../../static/images/petSearch.png';
import petHelp from '../../static/images/petHelp.png';
import petSaw from '../../static/images/petSaw.png';

import { LoginModal, RegisterModal } from '../../components/modals';

const images = [
    {
        url: petSearch,
        title: 'Ищу питомца',
        width: '30%',
    },
    {
        url: petHelp,
        title: 'Хочу помочь',
        width: '30%',
    },
    {
        url: petSaw,
        title: 'Нашел бездомного питомца',
        width: '40%',
    },
];

const useStyles = makeStyles(theme => ({
    root: {
        display: 'flex',
        flexWrap: 'wrap',
        minWidth: 300,
        width: '100%',
    },
    image: {
        position: 'relative',
        height: 150,
        [theme.breakpoints.down('xs')]: {
            width: '100% !important', // Overrides inline-style
            height: 100,
        },
        '&:hover, &$focusVisible': {
            border: '4px solid #2196f3',
            zIndex: 1,
            '& $imageBackdrop': {
                opacity: 0.15,
            },
            '& $imageMarked': {
                opacity: 0,
            },
            '& $imageTitle': {
                border: '4px solid #2196f3',
            },
        },
    },
    focusVisible: {},
    imageButton: {
        position: 'absolute',
        left: 0,
        right: 0,
        top: 0,
        bottom: 0,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        color: theme.palette.common.white,
    },
    imageSrc: {
        position: 'absolute',
        left: 0,
        right: 0,
        top: 0,
        bottom: 0,
        backgroundSize: 'cover',
        backgroundPosition: 'center 40%',
    },
    imageBackdrop: {
        position: 'absolute',
        left: 0,
        right: 0,
        top: 0,
        bottom: 0,
        backgroundColor: theme.palette.common.black,
        opacity: 0.4,
        transition: theme.transitions.create('opacity'),
    },
    imageTitle: {
        position: 'relative',
        padding: `${theme.spacing(2)}px ${theme.spacing(4)}px ${theme.spacing(1) + 6}px`,
    },
    imageMarked: {
        height: 3,
        width: 60,
        backgroundColor: '#2196f3',
        position: 'absolute',
        bottom: -2,
        left: 'calc(50% - 30px)',
        transition: theme.transitions.create('opacity'),
    },
}));

export default function Header() {
    const classes = useStyles();

    return (
        <div className={classes.root}>
            {/* <LoginModal open={true} handleCancel={() => {}} handleLogin={() => {}} /> */}
            {/* <RegisterModal open={true} handleCancel={() => {}} handleRegister={() => {}} /> */}
        </div>
    );
}